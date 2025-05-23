namespace ModernBaseLibrary.Core.IO
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Text;
    using System.Text.Json.Serialization;

    [JsonConverter(typeof(FullPathJsonConverter))]
    public readonly struct FullPath : IEquatable<FullPath>, IComparable<FullPath>
    {
        internal readonly string _value;

        private FullPath(string path)
        {
            // The checks are already performed in the static methods
            // No need to check if the path is null or absolute here
            Debug.Assert(path is not null);
            Debug.Assert(Path.IsPathFullyQualified(path));
            Debug.Assert(Path.GetFullPath(path) == path);
            _value = path;
        }

        public static FullPath Empty => default;

        [MemberNotNullWhen(returnValue: false, nameof(_value))]
        public bool IsEmpty => _value is null;

        public string Value => _value ?? "";

        public static implicit operator string(FullPath fullPath) => fullPath.ToString();

        public static bool operator ==(FullPath path1, FullPath path2) => path1.Equals(path2);
        public static bool operator !=(FullPath path1, FullPath path2) => !(path1 == path2);
        public static bool operator <(FullPath path1, FullPath path2) => path1.CompareTo(path2) < 0;
        public static bool operator >(FullPath path1, FullPath path2) => path1.CompareTo(path2) > 0;
        public static bool operator <=(FullPath path1, FullPath path2) => path1.CompareTo(path2) <= 0;
        public static bool operator >=(FullPath path1, FullPath path2) => path1.CompareTo(path2) >= 0;

        public static FullPath operator /(FullPath rootPath, string relativePath) => Combine(rootPath, relativePath);

        public FullPath Parent
        {
            get
            {
                var result = Path.GetDirectoryName(_value);
                if (result is null)
                    return Empty;

                return new FullPath(result);
            }
        }

        public string Name => Path.GetFileName(_value) ?? "";

        public string NameWithoutExtension => Path.GetFileNameWithoutExtension(_value) ?? "";

        public string Extension => Path.GetExtension(_value) ?? "";

        public int CompareTo(FullPath other) => FullPathComparer.Default.Compare(this, other);
        public int CompareTo(FullPath other, bool ignoreCase) => FullPathComparer.GetComparer(ignoreCase).Compare(this, other);

        public override bool Equals(object obj) => obj is FullPath path && Equals(path);

        public bool Equals(FullPath other) => FullPathComparer.Default.Equals(this, other);

        public bool Equals(FullPath other, bool ignoreCase) => FullPathComparer.GetComparer(ignoreCase).Equals(this, other);

        public override int GetHashCode() => FullPathComparer.Default.GetHashCode(this);

        public int GetHashCode(bool ignoreCase) => FullPathComparer.GetComparer(ignoreCase).GetHashCode(this);

        public override string ToString() => Value;

        public string MakePathRelativeTo(FullPath rootPath)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("The path is empty");
            }

            if (rootPath.IsEmpty)
            {
                return _value;
            }

            if (rootPath == this)
            {
                return ".";
            }

            return PathDifference(rootPath._value, _value, compareCase: FullPathComparer.Default.IsCaseSensitive);
        }

        private static string PathDifference(string path1, string path2, bool compareCase)
        {
            var directorySeparator = Path.DirectorySeparatorChar;

            int i;
            var si = -1;
            for (i = 0; (i <= path1.Length) && (i < path2.Length); ++i)
            {
                var c1 = i == path1.Length ? directorySeparator : path1[i];
                var c2 = path2[i];

                if ((c1 != c2) && (compareCase || (char.ToUpperInvariant(c1) != char.ToUpperInvariant(c2))))
                {
                    break;
                }

                if (c1 == directorySeparator)
                {
                    si = i;
                }
            }

            if (i == 0)
            {
                return path2;
            }

            if ((i == path1.Length + 1) && (i == path2.Length))
            {
                return string.Empty;
            }

            StringBuilder relPath = new StringBuilder();
            for (; i <= path1.Length; ++i)
            {
                char c = i == path1.Length ? directorySeparator : path1[i];
                if (c == directorySeparator)
                {
                    relPath.Append("..");
                    relPath.Append(directorySeparator);
                }
            }

            return relPath.Append(path2.AsSpan(si + 1)).ToString();
        }

        public bool IsChildOf(FullPath rootPath)
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Path is empty");
            }

            if (rootPath.IsEmpty)
            {
                throw new ArgumentException("Root path is empty", nameof(rootPath));
            }

            if (_value.Length <= rootPath._value.Length)
            {
                return false;
            }

            if (!_value.StartsWith(rootPath._value, StringComparison.Ordinal))
            {
                return false;
            }

            // rootpath: /a/b
            // current:  /a/b/c => true
            // current:  /a/b/  => false
            // current:  /a/bc  => false
            if (_value[rootPath._value.Length] == Path.DirectorySeparatorChar && _value.Length > rootPath._value.Length + 1)
            {
                return true;
            }

            return false;
        }

        public void CreateParentDirectory()
        {
            if (IsEmpty == true)
            {
                return;
            }

            string parent = Path.GetDirectoryName(Value);
            if (parent is not null)
            {
                Directory.CreateDirectory(parent);
            }
        }

        public FullPath ChangeExtension(string extension)
        {
            if (IsEmpty == true)
            {
                return Empty;
            }

            return new FullPath(Path.ChangeExtension(Value, extension));
        }

        public static FullPath GetTempPath() => FromPath(Path.GetTempPath());

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static FullPath GetTempFileName() => FromPath(Path.GetTempFileName());

        public static FullPath CreateTempFile() => FromPath(Path.GetTempFileName());

        public static FullPath GetFolderPath(Environment.SpecialFolder folder) => FromPath(Environment.GetFolderPath(folder));

        public static FullPath CurrentDirectory() => FromPath(Environment.CurrentDirectory);

        public static FullPath FromPath(string path)
        {
            if (PathInternal.IsExtended(path))
            {
                path = path[4..];
            }

            var fullPath = Path.GetFullPath(path);
            var fullPathWithoutTrailingDirectorySeparator = TrimEndingDirectorySeparator(fullPath);
            if (string.IsNullOrEmpty(fullPathWithoutTrailingDirectorySeparator))
            {
                return Empty;
            }

            return new FullPath(fullPathWithoutTrailingDirectorySeparator);
        }

        private static string TrimEndingDirectorySeparator(string path)
        {
            return Path.TrimEndingDirectorySeparator(path);
        }

        public static FullPath Combine(string rootPath, string relativePath) => FromPath(Path.Combine(rootPath, relativePath));
        public static FullPath Combine(string rootPath, string path1, string path2) => FromPath(Path.Combine(rootPath, path1, path2));
        public static FullPath Combine(string rootPath, string path1, string path2, string path3) => FromPath(Path.Combine(rootPath, path1, path2, path3));
        public static FullPath Combine(params string[] paths) => FromPath(Path.Combine(paths));

        public static FullPath Combine(FullPath rootPath, string relativePath)
        {
            if (rootPath.IsEmpty)
            {
                return FromPath(relativePath);
            }

            return FromPath(Path.Combine(rootPath._value, relativePath));
        }

        public static FullPath Combine(FullPath rootPath, string path1, string path2)
        {
            if (rootPath.IsEmpty)
            {
                return FromPath(Path.Combine(path1, path2));
            }

            return FromPath(Path.Combine(rootPath._value, path1, path2));
        }

        public static FullPath Combine(FullPath rootPath, params string[] paths)
        {
            if (rootPath.IsEmpty)
            {
                return FromPath(Path.Combine(paths));
            }

            return FromPath(Path.Combine(rootPath._value, Path.Combine(paths)));
        }

        public static FullPath Combine(FullPath rootPath, string path1, string path2, string path3)
        {
            if (rootPath.IsEmpty)
            {
                return FromPath(Path.Combine(path1, path2, path3));
            }

            return FromPath(Path.Combine(rootPath._value, path1, path2, path3));
        }

        public static FullPath FromFileSystemInfo(FileSystemInfo fsi)
        {
            if (fsi is null)
            {
                return Empty;
            }

            return FromPath(fsi.FullName);
        }

        public bool TryFindFirstAncestorOrSelf(Func<FullPath, bool> predicate, out FullPath result)
        {
            var current = this;
            while (!current.IsEmpty)
            {
                if (predicate(current))
                {
                    result = current;
                    return true;
                }

                current = current.Parent;
            }

            result = default;
            return false;
        }

        public bool TryFindFirstAncestor(Func<FullPath, bool> predicate, out FullPath result)
        {
            return Parent.TryFindFirstAncestorOrSelf(predicate, out result);
        }
    }
}
