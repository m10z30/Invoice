
using Invoice.Data;

namespace Invoice.Utils
{
    public class Numbers
    {
        private int number = -1;
        private IServiceScopeFactory _serviceScopeFactory;
        
        public Numbers(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<int> GetNumberAsync()
        {
            if (number == -1)
            {
                number = await GetLastNumber(_serviceScopeFactory);
            }
            lock(this){
                if (number == 100)
                {
                    number = 1;
                }
                return number++;
            }
        }

        public void ResetNumber()
        {
            number = 1;
        }

        private async Task<int> GetLastNumber(IServiceScopeFactory serviceScopeFactory)
        {
            using IServiceScope scope = serviceScopeFactory.CreateScope();
            var metaData = scope.ServiceProvider.GetRequiredService<MetaDataUtil>();
            var context  = scope.ServiceProvider.GetRequiredService<DataContext>();
            return await metaData.GetNumber(context);
        }
    }
}