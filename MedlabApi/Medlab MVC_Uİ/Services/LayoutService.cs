using Medlab.Core.Repositories;

namespace Medlab_MVC_Uİ.Services
{
    public class LayoutService
    {
        private readonly ISettingRepository _settingRepository;

        public LayoutService(ISettingRepository settingRepository)
        {
            this._settingRepository = settingRepository;
        }

        public Dictionary<string,string> GetSetting()
        {
          return  _settingRepository.GetSettingDictionary();
        }

    }
}
