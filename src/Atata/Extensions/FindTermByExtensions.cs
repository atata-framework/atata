using System;
namespace Atata
{
    public static class FindTermByExtensions
    {
        public static Type ResolveFindAttributeType(this FindTermBy by)
        {
            switch (by)
            {
                case FindTermBy.Id:
                    return typeof(FindByIdAttribute);
                case FindTermBy.Name:
                    return typeof(FindByNameAttribute);
                case FindTermBy.Class:
                    return typeof(FindByClassAttribute);
                case FindTermBy.Label:
                    return typeof(FindByLabelAttribute);
                case FindTermBy.Content:
                    return typeof(FindByContentAttribute);
                case FindTermBy.Value:
                    return typeof(FindByValueAttribute);
                case FindTermBy.ContentOrValue:
                    return typeof(FindByContentOrValueAttribute);
                case FindTermBy.ColumnHeader:
                    return typeof(FindByColumnHeaderAttribute);
                case FindTermBy.Title:
                    return typeof(FindByTitleAttribute);
                case FindTermBy.FieldSet:
                    return typeof(FindByFieldSetAttribute);
                case FindTermBy.Placeholder:
                    return typeof(FindByPlaceholderAttribute);
                case FindTermBy.DescriptionTerm:
                    return typeof(FindByDescriptionTermAttribute);
                case FindTermBy.ChildContent:
                    return typeof(FindByChildContentAttribute);
                default:
                    throw ExceptionFactory.CreateForUnsupportedEnumValue(by, "by");
            }
        }
    }
}
