using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectTemplateWizard.ExtensionMethods
{
    internal static class EnumExtension
    {
        public static T GetAttribute<T>(this Enum en) where T : Attribute
        {
            var attributes = en.GetType().GetMember(en.ToString())[0].GetCustomAttributes(typeof(T), false);

            return attributes.Length > 0 ? (T) attributes[0] : null;
        }

        public static string GetDisplayName(this Enum en)
        {
            var attr = en.GetAttribute<DisplayAttribute>();

            if (attr != null)
            {
                var name = attr.Name;

                if (!string.IsNullOrWhiteSpace(name))
                {
                    return name;
                }
            }

            return en.ToString();
        }

        public static string GetDisplayDescription(this Enum en)
        {
            var attr = en.GetAttribute<DisplayAttribute>();

            if (attr != null)
            {
                var description = attr.Description;

                if (!string.IsNullOrWhiteSpace(description))
                {
                    return description;
                }
            }

            return en.ToString();
        }
    }
}