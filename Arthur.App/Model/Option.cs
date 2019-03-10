using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arthur.App.Model
{
    public class Option
    {

        public int Id { get; set; }

        /// <summary>
        /// 键名
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Value { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(100)]
        public string Remark { get; set; }

        public Option()
        {
            this.Id = -1;
        }

        public Option(string key, string value) : this(key, value, "")
        {
        }

        public Option(string key, string value, string remark)
        {
            Key = key;
            Value = value;
            Remark = remark;
        }
    }
}
