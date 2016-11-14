using System;

namespace Atata
{
    /// <summary>
    /// Specifies the default finding strategy of a control. Can be applied to the control class and assembly.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Assembly, AllowMultiple = true)]
    public class ControlFindingAttribute : Attribute
    {
        public ControlFindingAttribute(FindTermBy by)
            : this(by.ResolveFindAttributeType())
        {
        }

        public ControlFindingAttribute(Type findAttributeType)
        {
            FindAttributeType = findAttributeType.
                CheckNotNull(nameof(findAttributeType)).
                Check(x => x.IsSubclassOf(typeof(FindAttribute)), nameof(findAttributeType), $"{findAttributeType.FullName} type is not a subclass of {nameof(FindAttribute)}.");
        }

        /// <summary>
        /// Gets the type of the attribute to use for the control finding. Type should be inherited from <see cref="FindAttribute"/>.
        /// </summary>
        public Type FindAttributeType { get; private set; }

        /// <summary>
        /// Gets or sets the type of the control (e.g.: typeof(LinkControl&lt;&gt;), typeof(EditableField&lt;,&gt;)).
        /// </summary>
        public Type ControlType { get; set; }

        /// <summary>
        /// Gets or sets the type of the parent component.
        /// </summary>
        public Type ParentComponentType { get; set; }

        public FindAttribute CreateFindAttribute()
        {
            return (FindAttribute)ActivatorEx.CreateInstance(FindAttributeType);
        }
    }
}
