﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HighPerformance.Messaging
{
    public class Message
    {
        private const string PString = "S$";
        private const string PInteger = "I$";
        private const string PLong = "L$";
        private const string PBoolean = "B$";

        private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>();

        public string Encode()
        {
            var builder = new StringBuilder();
            builder.Append("SMA");
            builder.Append('\t');
            builder.Append(Length);
            builder.Append('\t');
            builder.Append(Type);
            builder.Append('\t');
            builder.Append(Tag);
            builder.Append('\t');
            builder.Append(_parameters.Count);
            builder.Append('\t');
            foreach (var parameter in _parameters)
            {
                builder.Append(parameter.Key);
                builder.Append('\t');
                builder.Append(parameter.Value);
                builder.Append('\t');
            }
            return builder.ToString();
        }

        public void Decode(string reader)
        {
            var parts = reader.Split('\t');
            var index = 0;
            if (parts[index++] != "SMA")
                throw new Exception();
            Length = int.Parse(parts[index++]);
            Type = int.Parse(parts[index++]);
            Tag = int.Parse(parts[index++]);
            var count = int.Parse(parts[index++]);
            for (var i = 0; i < count; i++)
            {
                var key = parts[index++];
                var value = parts[index++];
                _parameters.Add(key, value);
            }
        }

        public int Type { get; set; }
        public int Tag { get; set; }
        public int Length { get; set; }

        public string this[string key]
        {
            get { return _parameters[key]; }
            set { _parameters[key] = value; }
        }

        public string GetString(string key)
        {
            return _parameters[PString + key];
        }

        public void SetString(string key, string value)
        {
            _parameters[PString + key] = value;
        }

        public int GetInt(string key)
        {
            return int.Parse(_parameters[PInteger + key]);
        }

        public void SetInt(string key, int value)
        {
            _parameters[PInteger + key] = value.ToString();
        }

        public long GetLong(string key)
        {
            return long.Parse(_parameters[PLong + key]);
        }

        public void SetLong(string key, long value)
        {
            _parameters[PLong + key] = value.ToString();
        }

        public bool GetBool(string key)
        {
            return bool.Parse(_parameters[PBoolean + key]);
        }

        public void SetBool(string key, bool value)
        {
            _parameters[PBoolean + key] = value.ToString();
        }

        public void Merge(Message message)
        {
            foreach (var parameter in message._parameters)
                _parameters.Add(parameter.Key, parameter.Value);
        }
    }
}