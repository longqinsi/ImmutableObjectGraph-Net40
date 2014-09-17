namespace ImmutableObjectGraph.Tests {
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    
    using Xunit;

    public class TreeNodeTests {
        [Fact]
        public void TreeConstruction() {
            var root = TreeNode.Create("temp", @"c:\temp\", Optional<System.Boolean>.Missing, Optional<ImmutableHashSet<String>>.Missing, Optional<ImmutableList<TreeNode>>.Missing).With(Optional<string>.Missing, Optional<string>.Missing, Optional<bool>.Missing, Optional<ImmutableHashSet<String>>.Missing, ImmutableList.Create(
                TreeNode.Create("a.cs", @"c:\temp\a.cs", Optional<System.Boolean>.Missing, Optional<ImmutableHashSet<String>>.Missing, Optional<ImmutableList<TreeNode>>.Missing),
                TreeNode.Create("subfolder", @"c:\temp\subfolder\", Optional<System.Boolean>.Missing, Optional<ImmutableHashSet<String>>.Missing, Optional<ImmutableList<TreeNode>>.Missing).With(Optional<string>.Missing, Optional<string>.Missing, Optional<bool>.Missing, Optional<ImmutableHashSet<String>>.Missing, ImmutableList.Create(
                    TreeNode.Create("g.h", @"c:\temp\subfolder\g.h", Optional<System.Boolean>.Missing, Optional<ImmutableHashSet<String>>.Missing, Optional<ImmutableList<TreeNode>>.Missing)))));

            Assert.Equal("temp", root.Caption);
            Assert.Equal("a.cs", root.Children[0].Caption);
            Assert.Equal("subfolder", root.Children[1].Caption);
            Assert.Equal("g.h", root.Children[1].Children[0].Caption);

            var builder = root.ToBuilder();
            builder.Children.Add(TreeNode.Create("b.cs", @"c:\temp\b.cs", Optional<System.Boolean>.Missing, Optional<ImmutableHashSet<String>>.Missing, Optional<ImmutableList<TreeNode>>.Missing));
            var root2 = builder.ToImmutable();
            Assert.Equal(3, root2.Children.Count);

            builder.Children.Remove(builder.Children[0]);
            var root3 = builder.ToImmutable();
            Assert.Equal(2, root3.Children.Count);
            Assert.Equal(root2.Children[1], root3.Children[0]);
        }
    }

    [DebuggerDisplay("{FilePath}")]
    partial class TreeNode {
        static partial void CreateDefaultTemplate(ref TreeNode.Template template) {
            template.Children = ImmutableList.Create<TreeNode>();
            template.Visible = true;
            template.Attributes = ImmutableHashSet.Create<string>(StringComparer.OrdinalIgnoreCase);
        }

        [DebuggerDisplay("{FilePath}")]
        partial class Builder {
        }
    }
}
