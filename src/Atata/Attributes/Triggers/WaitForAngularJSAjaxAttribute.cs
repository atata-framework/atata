using System;

namespace Atata
{
    /// <summary>
    /// Indicates that the waiting should be performed until the AngularJS (v1.*) AJAX is completed. By default occurs after the click.
    /// </summary>
    public class WaitForAngularJSAjaxAttribute : TriggerAttribute
    {
        public WaitForAngularJSAjaxAttribute(TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
            : base(on, priority)
        {
        }

        protected internal override void Execute<TOwner>(TriggerContext<TOwner> context)
        {
            context.Log.Start("Wait for AngularJS AJAX execution", LogLevel.Trace);

            bool completed = context.Driver.Try().Until(
                x => (bool)context.Driver.ExecuteScript(@"
try {
    if (document.readyState !== 'complete') {
        return false;
    }
    if (window.angular) {
        var injector = window.angular.element('body').injector();
    
        var $rootScope = injector.get('$rootScope');
        var $http = injector.get('$http');
        var $timeout = injector.get('$timeout');
    
        if ($rootScope.$$phase === '$apply' || $rootScope.$$phase === '$digest' || $http.pendingRequests.length !== 0) {
            return false;
        }
    }
    return true;
} catch (err) {
  return false;
}"));

            context.Log.EndSection();
            if (!completed)
                throw new TimeoutException("Timed out waiting for AngularJS AJAX call to complete.");
        }
    }
}
