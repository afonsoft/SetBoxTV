﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SetBoxTV.VideoPlayer.Model
{

    /// <summary>
    /// Files
    /// </summary>
    [Android.Runtime.Preserve(AllMembers = true)]
    public class FileCheckSum
    {
        [Newtonsoft.Json.JsonConstructor]
        public FileCheckSum()
        {
        }

        /// <summary>
        /// Nome do Arquivo
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Tipo do Arquivo
        /// </summary>
        public string extension { get; set; }
        /// <summary>
        /// Tamanho
        /// </summary>
        public long size { get; set; }
        /// <summary>
        /// Url para download do arquivo
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// CheckSum para verificar se foi modificado o arquivo.
        /// </summary>
        public string checkSum { get; set; }
        /// <summary>
        /// Descrição do Arquivo
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// Data de Criação do Arquivo no Servidor
        /// </summary>
        public DateTime creationDateTime { get; set; }
    }
}
