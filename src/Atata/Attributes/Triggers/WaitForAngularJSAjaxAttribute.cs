namespace Atata
{
    /// <summary>
    /// Indicates that the waiting should be performed until the AngularJS (v1.*) AJAX is completed.
    /// By default occurs after the click.
    /// </summary>
    public class WaitForAngularJSAjaxAttribute : WaitForScriptAttribute
    {
        public WaitForAngularJSAjaxAttribute(TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        protected override string BuildScript<TOwner>(TriggerContext<TOwner> context)
            => @"
try {
    if (document.readyState !== 'complete') {
        return false;
    }
    if (window.angular) {
        var injector = window.angular.element('body').injector();
    
        var $rootScope = injector.get('$rootScope');
        var $http = injector.get('$http');

        if ($rootScope.$$phase === '$apply' || $rootScope.$$phase === '$digest' || $http.pendingRequests.length !== 0) {
            return false;
        }
    }
    return true;
} catch (err) {
  return false;
}";
    }
}
