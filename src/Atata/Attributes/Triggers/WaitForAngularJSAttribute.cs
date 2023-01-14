namespace Atata
{
    /// <summary>
    /// Indicates to wait until AngularJS (v1) has finished rendering and has no outstanding HTTP calls.
    /// By default occurs after the click.
    /// </summary>
    public class WaitForAngularJSAttribute : WaitForScriptAttribute
    {
        public WaitForAngularJSAttribute(TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
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
