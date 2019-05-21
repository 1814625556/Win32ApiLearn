using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FileUploadDownLoad.HttpEntity
{
    public class Upload360Entity
    {
        /// <summary>
        /// 版本必填
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }
        /// <summary>
        /// token必填
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// 下载地址必填
        /// </summary>
        [JsonProperty("download")]
        public string Download { get; set; }

        [JsonProperty("stamp")]
        public string Stamp;
        
        [JsonProperty("publi")]
        public string Publi;
        
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("intro")]
        public string Intro { get; set; }

        [JsonProperty("softmgr_id")]
        public string Softmgr_id { get; set; }

        [JsonProperty("push2softmgr")]
        public string Push2softmgr { get; set; }


    }
}
