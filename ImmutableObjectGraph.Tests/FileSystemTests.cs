namespace ImmutableObjectGraph.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Xunit;

    public class FileSystemTests
    {
        private FileSystemDirectory root;

        public FileSystemTests()
        {
            var builder = FileSystemDirectory.Create("c:", Optional<System.Collections.Immutable.ImmutableSortedSet<FileSystemEntry>>.Missing).ToBuilder();
            builder.Children.Add(
                FileSystemFile.Create("a.cs", Optional<System.Collections.Immutable.ImmutableHashSet<string>>.Missing));
            builder.Children.Add(FileSystemFile.Create("b.cs", Optional<System.Collections.Immutable.ImmutableHashSet<string>>.Missing));
            builder.Children.Add(FileSystemDirectory.Create("c", ImmutableSortedSet.Create<FileSystemEntry>(
                FileSystemFile.Create("d.cs", Optional<System.Collections.Immutable.ImmutableHashSet<string>>.Missing))));
            this.root = builder.ToImmutable();
        }

        [Fact]
        public void RecursiveDirectories()
        {
            var emptyRoot = FileSystemDirectory.Create("c:", Optional<System.Collections.Immutable.ImmutableSortedSet<FileSystemEntry>>.Missing);
            Assert.True(emptyRoot is IEnumerable<FileSystemEntry>);
            Assert.Equal(0, EnumerableV20.Count(emptyRoot)); // using Linq exercises the enumerable

            Assert.Equal(3, EnumerableV20.Count(this.root));  // use Linq to exercise enumerator
            Assert.Equal(1, EnumerableV20.Count(EnumerableV20.OfType<FileSystemDirectory>(this.root)));
        }

        [Fact]
        public void NonRecursiveFiles()
        {
            Assert.False(FileSystemFile.Create("a", Optional<System.Collections.Immutable.ImmutableHashSet<string>>.Missing) is System.Collections.IEnumerable);
        }

        [Fact]
        public void TypeConversion()
        {
            FileSystemFile file = FileSystemFile.Create("a", Optional<System.Collections.Immutable.ImmutableHashSet<string>>.Missing);
            FileSystemDirectory folder = file.ToFileSystemDirectory(Optional<System.Collections.Immutable.ImmutableSortedSet<FileSystemEntry>>.Missing);
            Assert.Equal(file.PathSegment, folder.PathSegment);
            FileSystemFile fileAgain = folder.ToFileSystemFile(Optional<System.Collections.Immutable.ImmutableHashSet<System.String>>.Missing);
            Assert.Equal(file.PathSegment, fileAgain.PathSegment);
        }

        [Fact]
        public void ReplaceDescendentUpdatesProperty()
        {
            //var leafToModify = EnumerableV20.Single(EnumerableV20.Single(EnumerableV20.OfType<FileSystemDirectory>(this.root), c => c.PathSegment == "c").Children);
            //var updatedLeaf = leafToModify.With("e.cs");
            //var builder = this.root.ToBuilder();
            //var updatedTree = this.root.ReplaceDescendent(leafToModify, updatedLeaf);
            //Assert.Equal(this.root.PathSegment, updatedTree.PathSegment);
            //var leafFromUpdatedTree = EnumerableV20.Single(EnumerableV20.Single(EnumerableV20.OfType<FileSystemDirectory>(updatedTree), c => c.PathSegment == "c").Children);
            //Assert.Equal(updatedLeaf.PathSegment, leafFromUpdatedTree.PathSegment);
        }

        [Fact]
        public void ReplaceDescendentChangesType()
        {
            //var leafToModify = EnumerableV20.Single(EnumerableV20.Single(EnumerableV20.OfType<FileSystemDirectory>(this.root), c => c.PathSegment == "c").Children);
            //var updatedLeaf = leafToModify.ToFileSystemDirectory().WithPathSegment("f");
            //var updatedTree = this.root.ReplaceDescendent(leafToModify, updatedLeaf);
            //var leafFromUpdatedTree = EnumerableV20.Single(EnumerableV20.Single(EnumerableV20.OfType<FileSystemDirectory>(updatedTree), c => c.PathSegment == "c").Children);
            //Assert.IsType<FileSystemDirectory>(leafFromUpdatedTree);
            //Assert.Equal(updatedLeaf.PathSegment, leafFromUpdatedTree.PathSegment);
        }

        [Fact(Skip = "It currently fails")]
        public void ReplaceDescendentNotFound()
        {
            //Assert.Throws<ArgumentException>(() => this.root.ReplaceDescendent(FileSystemFile.Create("nonexistent"), FileSystemFile.Create("replacement")));
        }
    }

    [DebuggerDisplay("{FullPath}")]
    partial class FileSystemFile
    {
        static partial void CreateDefaultTemplate(ref FileSystemFile.Template template)
        {
            template.Attributes = ImmutableHashSet.Create<string>(StringComparer.OrdinalIgnoreCase);
        }
    }

    [DebuggerDisplay("{FullPath}")]
    partial class FileSystemDirectory
    {
        public override string FullPath
        {
            get { return base.FullPath + Path.DirectorySeparatorChar; }
        }

        static partial void CreateDefaultTemplate(ref FileSystemDirectory.Template template)
        {
            template.Children = ImmutableSortedSet.Create<FileSystemEntry>(SiblingComparer.Instance);
        }
    }

    partial class FileSystemEntry
    {
        public virtual string FullPath
        {
            get
            {
                // TODO: when we get properties that point back to the root, fix this to include the full path.
                return this.PathSegment;
            }
        }

        public class SiblingComparer : IComparer<FileSystemEntry>
        {
            public static SiblingComparer Instance = new SiblingComparer();

            private SiblingComparer()
            {
            }

            public int Compare(FileSystemEntry x, FileSystemEntry y)
            {
                return StringComparer.OrdinalIgnoreCase.Compare(x.PathSegment, y.PathSegment);
            }
        }
    }
}
