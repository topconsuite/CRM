using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TopSys.TopConWeb.SharedKernel.Events;

namespace TopSys.TopConWeb.SharedKernel.Validation
{
    public static class AssertionConcern
    {
        public static bool IsSatisfiedBy(params DomainNotification[] validations)
        {
            var notificationsNotNull = validations.Where(validation => validation != null);
            NotifyAll(notificationsNotNull);

            return notificationsNotNull.Count().Equals(0);
        }

        public static void Notify(string property, string message)
        {
            DomainEvent.Raise<DomainNotification>(new DomainNotification(property, message));
        }

        private static void NotifyAll(IEnumerable<DomainNotification> notifications)
        {
            notifications.ToList().ForEach(validation =>
            {
                DomainEvent.Raise<DomainNotification>(validation);
            });
        }

        public static DomainNotification AssertLength(string stringValue, int minimum, int maximum, string property, string message)
        {
            int length = stringValue.Trim().Length;

            return (length < minimum || length > maximum) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertMatches(string pattern, string stringValue, string property, string message)
        {
            Regex regex = new Regex(pattern);

            return (!regex.IsMatch(stringValue)) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertNotEmpty(string stringValue, string property, string message)
        {
            return (string.IsNullOrEmpty(stringValue)) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertNotNull(object object1, string property, string message)
        {

            return (object1 == null) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertTrue(bool boolValue, string property, string message)
        {
            return (!boolValue) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertAreEquals(int value, int match, string property, string message)
        {
            return (!(value == match)) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertAreEquals(float value, float match, string property, string message)
        {
            return (!(value == match)) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertAreEquals(decimal value, decimal match, string property, string message)
        {
            return (!(value == match)) ?
                new DomainNotification(property, message) : null;
        }
        public static DomainNotification AssertAreNotEquals(int value, int match, string property, string message)
        {
            return (value == match) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertAreNotEquals(float value, float match, string property, string message)
        {
            return (value == match) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertAreNotEquals(decimal value, decimal match, string property, string message)
        {
            return (value == match) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertAreEquals(string value, string match, string property, string message)
        {
            return (!(value == match)) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertIsGreaterThan(int value1, int value2, string property, string message)
        {
            return (!(value1 > value2)) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertIsGreaterThan(decimal value1, decimal value2, string property, string message)
        {
            return (!(value1 > value2)) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertIsGreaterThan(float value1, float value2, string property, string message)
        {
            return (!(value1 > value2)) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertIsGreaterThan(DateTime? value1, DateTime? value2, string property, string message)
        {
            return (!(value1 > value2)) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertIsGreaterOrEqualThan(int value1, int value2, string property, string message)
        {
            return (!(value1 >= value2)) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertIsGreaterOrEqualThan(decimal value1, decimal value2, string property, string message)
        {
            return (!(value1 >= value2)) ?
                new DomainNotification(property, message) : null;
        }
        
        public static DomainNotification AssertIsGreaterOrEqualThan(float value1, float value2, string property, string message)
        {
            return (!(value1 >= value2)) ?
                new DomainNotification(property, message) : null;
        }

        public static DomainNotification AssertIsGreaterOrEqualThan(DateTime? value1, DateTime? value2, string property, string message)
        {
            return (!(value1 >= value2)) ?
                new DomainNotification(property, message) : null;
        }
    }
}
