using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace workshop.tests.Tools
{

    public class RecursiveComparisonConstraint : Constraint
    {
        public object Expected { get; }

        public override string Description => "Compares the properties of two objects recursively";

        public RecursiveComparisonConstraint(object expected)
        {
            Expected = expected;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            if (actual == null || Expected == null)
            {
                return new ConstraintResult(this, actual, Equals(actual, Expected));
            }

            return new ConstraintResult(this, actual, Compare(actual, Expected));
        }

        public static bool Compare<T>(T obj1, T obj2) where T : class
        {
            if (obj1 == null || obj2 == null)
            {
                return obj1 == obj2; // If both are null, they are equal; otherwise not
            }

            // Check if they are the same reference (optimization)
            if (ReferenceEquals(obj1, obj2)) return true;

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var value1 = property.GetValue(obj1);
                var value2 = property.GetValue(obj2);

                // If the property is a class (excluding string), perform recursive comparison
                if (value1 != null && value2 != null && property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    if (!Compare(value1, value2)) // Recursive comparison for nested objects
                    {
                        return false;
                    }
                }
                else
                {
                    // For primitive types or value types, use default equality check
                    if (value1 == null && value2 == null)
                    {
                        continue; // If both are null, consider them equal
                    }

                    if (value1 == null || value2 == null || !Equals(value1, value2)) // Use Object.Equals for null-safe comparison
                    {
                        return false;
                    }
                }
            }

            return true; // If no differences found, the objects are considered equal
        }
    }
}
