namespace Atata
{
    /// <summary>
    /// Represents the behavior for drag and drop using JavaScript.
    /// The script simulates drag and drop by dispatching DOM events: 'dragstart', 'dragenter', 'dragover', 'drop' and 'dragend'.
    /// </summary>
    public class DragAndDropUsingScriptAttribute : DragAndDropBehaviorAttribute
    {
        private const string Script =
@"var src=arguments[0],tgt=arguments[1];var dataTransfer={dropEffect:'',effectAllowed:'all',files:[],items:{},types:[],
setData:function(format,data){this.items[format]=data;this.types.append(format);},
getData:function(format){return this.items[format];},
clearData:function(format){}};
var emit=function(event,target){var evt=document.createEvent('Event');evt.initEvent(event,true,false);
evt.dataTransfer=dataTransfer;target.dispatchEvent(evt);};
emit('dragstart',src);emit('dragenter',tgt);emit('dragover',tgt);emit('drop',tgt);emit('dragend',src);";

        public override void Execute<TOwner>(IControl<TOwner> component, IControl<TOwner> target)
        {
            AtataContext.Current.Driver.ExecuteScript(Script, component.Scope, target.Scope);
        }
    }
}
