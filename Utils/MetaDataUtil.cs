using Invoice.Data;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Utils
{
    public static class MetaDataUtil
    {
        public static async Task<int> GetNumber(DataContext context, int tries = 0)
        {
            using var transaction = context.Database.BeginTransaction(System.Data.IsolationLevel.Serializable);
            try
            {
                await transaction.CreateSavepointAsync("transaction"); // begin transaction


                var data = await context.MetaDatas.Where(m => m.Name == "Count").FirstOrDefaultAsync();
                if (data == null)
                {
                    throw new Exception("No initail metadata");
                }

                if(data.Value == 100)
                {
                    data.Value = 1;
                }
                else
                {
                    data.Value += 1;
                }

                await context.SaveChangesAsync();
                await transaction.CommitAsync(); // commit transaction
                return data.Value;
            }
            catch (Exception)
            {
                await transaction.RollbackToSavepointAsync("transaction"); // rollback
                if (tries > 10)
                {
                    throw new Exception("reach max tries");
                }
                return await GetNumber(context, tries+=1);
            }

        }

        public static async Task ResetNumber(DataContext context, int tries = 0)
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                await transaction.CreateSavepointAsync("transaction"); // begin transaction

                var data = await context.MetaDatas.Where(m => m.Name == "Count").FirstOrDefaultAsync();
                if (data == null)
                {
                    throw new Exception("No initail metadata");
                }

                data.Value = 0;
                await context.SaveChangesAsync();

                await transaction.CommitAsync(); // commit transaction
            }
            catch (Exception) 
            {
                await transaction.RollbackToSavepointAsync("transaction"); // rollback
                if (tries > 10)
                {
                    throw new Exception("reach max tries");
                }
                await ResetNumber(context, tries+=1);
            }
        }
    }
}