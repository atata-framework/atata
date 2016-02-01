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
                case FindTermBy.ContentOrValue:
                    return typeof(FindByContentAttribute);
                case FindTermBy.ColumnHeader:
                    return typeof(FindByColumnAttribute);
                default:
                    throw ExceptionsFactory.CreateForUnsupportedEnumValue(by, "by");
            }
        }
    }
}
