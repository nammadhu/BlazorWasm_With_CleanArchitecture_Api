namespace BlazorWebApp.Shared.Services
    {

    public class CommonService
        {
        //private readonly NavigationManager _navigationManager;
        //private readonly FeatureConfig _featureConfig;

        public CommonService()//NavigationManager navigationManager)//, FeatureConfig featureConfig)
            {
            //_navigationManager = navigationManager;
            //_featureConfig = featureConfig;
            }

        public bool IsFeatureEnabled(string featureName)
            {
            return true;
            //var currentDomain = _navigationManager.Uri.Host;
            //return _featureConfig.DomainFeaturesMap.ContainsKey(currentDomain) &&
            //    _featureConfig.DomainFeaturesMap[currentDomain].Contains(featureName);
            }
        }

    }
