namespace Demo {
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    
    using ImmutableObjectGraph;

    class Program {
        static void Main(string[] args) {
            var me = Contact.Create("Andrew Arnott", "andrewarnott@gmail.com");
            var builder = me.ToBuilder();
            builder.Email = "thisistheplace@hotmail.com";
            var myself = builder.ToImmutable();
            builder = me.ToBuilder();
            builder.Email = "andrewarnott@live.com";
            var i = builder.ToImmutable();

            var message = Message.Create(
                me,
                ImmutableList.Create(myself),
                Optional<System.Collections.Immutable.ImmutableList<Contact>>.Missing,
                Optional<System.Collections.Immutable.ImmutableList<Contact>>.Missing,
                "Hello, World",
                "What's happening?");

            var messageBuilder = message.ToBuilder();
            messageBuilder.Author.Name = "And again, myself";
            messageBuilder.Author.Email = "oh@so.mutable";
            messageBuilder.To.Add(i);

            var updatedMessage = messageBuilder.ToImmutable();
        }
    }

    [DebuggerDisplay("{Name,nq} <{Email,nq}>")]
    partial class Contact {
        [DebuggerDisplay("{Name,nq} <{Email,nq}>")]
        partial class Builder {
        }
    }
}
