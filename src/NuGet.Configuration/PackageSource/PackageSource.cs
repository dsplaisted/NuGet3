﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace NuGet.Configuration
{
    public class PackageSource : IEquatable<PackageSource>
    {
        /// <summary>
        /// The feed version for NuGet prior to v3.
        /// </summary>
        public const int DefaultProtocolVersion = 2;

        private readonly int _hashCode;
        private bool? _isHttp;

        public string Name { get; private set; }

        public string Source { get; set; }

        /// <summary>
        /// This does not represent just the NuGet Official Feed alone
        /// It may also represent a Default Package Source set by Configuration Defaults
        /// </summary>
        public bool IsOfficial { get; set; }

        public bool IsMachineWide { get; set; }

        public bool IsEnabled { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool IsPasswordClearText { get; set; }

        public string Description { get; set; }

        public bool IsPersistable { get; private set; }

        /// <summary>
        /// Gets or sets the protocol version of the source. Defaults to 2.
        /// </summary>
        public int ProtocolVersion { get; set; } = DefaultProtocolVersion;

        public bool IsHttp
        {
            get
            {
                if (!_isHttp.HasValue)
                {
                    _isHttp = Source.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                              Source.StartsWith("https://", StringComparison.OrdinalIgnoreCase);
                }

                return _isHttp.Value;
            }
        }

        /// <summary>
        /// Gets the <see cref="ISettings"/> that this source originated from. May be null.
        /// </summary>
        public ISettings Origin { get; set; }

        public PackageSource(string source)
            :
                this(source, source, isEnabled: true)
        {
        }

        public PackageSource(string source, string name)
            :
                this(source, name, isEnabled: true)
        {
        }

        public PackageSource(string source, string name, bool isEnabled)
            : this(source, name, isEnabled, isOfficial: false)
        {
        }

        public PackageSource(
            string source,
            string name,
            bool isEnabled,
            bool isOfficial,
            bool isPersistable = true)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            Name = name;
            Source = source;
            IsEnabled = isEnabled;
            IsOfficial = isOfficial;
            IsPersistable = isPersistable;
            _hashCode = Name.ToUpperInvariant().GetHashCode() * 3137 + Source.ToUpperInvariant().GetHashCode();
        }

        public bool Equals(PackageSource other)
        {
            if (other == null)
            {
                return false;
            }

            return Name.Equals(other.Name, StringComparison.CurrentCultureIgnoreCase) &&
                   Source.Equals(other.Source, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            var source = obj as PackageSource;
            if (source != null)
            {
                return Equals(source);
            }
            return base.Equals(obj);
        }

        public override string ToString()
        {
            return Name + " [" + Source + "]";
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public PackageSource Clone()
        {
            return new PackageSource(Source, Name, IsEnabled, IsOfficial, IsPersistable)
                {
                    Description = Description,
                    UserName = UserName,
                    Password = Password,
                    IsPasswordClearText = IsPasswordClearText,
                    IsMachineWide = IsMachineWide,
                    ProtocolVersion = ProtocolVersion
                };
        }
    }
}
