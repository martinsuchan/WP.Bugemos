using System;
using System.IO.IsolatedStorage;

namespace UtilityBelt
{
    // Encapsulates a key/value pair stored in Isolated Storage ApplicationSettings
    public class Setting<T>
    {
        private readonly string name;
        private T value;
        private readonly T defaultValue;
        private bool hasValue;

        public Setting(string name, T defaultValue)
        {
            this.name = name;
            this.defaultValue = defaultValue;
        }

        public T Value
        {
            get
            {
                // Check for the cached value
                if (!hasValue)
                {
                    try
                    {
                        // Try to get the value from Isolated Storage
                        if (!IsolatedStorageSettings.ApplicationSettings.TryGetValue(name, out value))
                        {
                            // It hasn’t been set yet
                            value = defaultValue;
                            IsolatedStorageSettings.ApplicationSettings[name] = value;
                        }
                    }
                    catch (Exception e)
                    {
                        value = defaultValue;
                        IsolatedStorageSettings.ApplicationSettings[name] = value;
                    }
                    hasValue = true;
                }
                return value;
            }
            set
            {
                // Save the value to Isolated Storage
                IsolatedStorageSettings.ApplicationSettings[name] = value;
                this.value = value;
                hasValue = true;
            }
        }

        public T DefaultValue
        {
            get { return defaultValue; }
        }

        // “Clear” cached value:
        public void ForceRefresh()
        {
            hasValue = false;
        }

        // save cached value to storage
        public void ForceSave()
        {
            if (hasValue)
            {
                IsolatedStorageSettings.ApplicationSettings[name] = value;
            }
            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", name, Value);
        }
    }
}
