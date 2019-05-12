using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App
{
    public class CacheHelper
    {
        public static object GetValue(string key)
        {
            var item = Data.CacheData.CacheItems.FirstOrDefault(o => o.Key == key);
            if (item != null)
            {
                if (item.CreateTime + item.ExpireTime > DateTime.Now)
                {
                    Data.CacheData.CacheItems.First(o => o.Key == key).CreateTime = DateTime.Now;
                    return item.Value;
                }
                else //过期
                {
                    Data.CacheData.CacheItems.Remove(item);
                }
            }
            return null;
        }

        public static void SetValue(string key, object value)
        {
            SetValue(key, value, new TimeSpan(1, 0, 0));
        }

        public static void SetValue(string key, object value, TimeSpan expireTime)
        {
            if (Data.CacheData.CacheItems.Count(o => o.Key == key) > 0)
            {
                Data.CacheData.CacheItems.First(o => o.Key == key).CreateTime = DateTime.Now;
                Data.CacheData.CacheItems.First(o => o.Key == key).Value = value;
            }
            else
            {
                Data.CacheData.CacheItems.Add(new CacheItem() { Key = key, Value = value, CreateTime = DateTime.Now, ExpireTime = expireTime });
            }
        }
    }
}
