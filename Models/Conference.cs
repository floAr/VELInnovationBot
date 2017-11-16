using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
#pragma warning disable 649

// The SandwichOrder is the simple form you want to fill out.  It must be serializable so the bot can be stateless.
// The order of fields defines the default order in which questions will be asked.
// Enumerations shows the legal options for each field in the SandwichOrder and the order is the order values will be presented 
// in a conversation.
namespace Microsoft.Bot.Sample.FormBot
{
    public enum ConfTarget
    {
      None, Learning, Recruiting, Networking  
    };
    
    public enum ConfType
    {
      None, Exhibition, HighLevel, DeepDive, Workshop
    };
    
    [Serializable]
    public class ConferenceEntry
    {
        public String ConferenceName;
        public String Website;
        public DateTime? Date;
        public List<ConfTarget> Goals;
        public List<ConfType> ContentType;
        public List<String> Tags;      
        [Prompt("Would you recommend to {&}? {||}")]  
        public bool? Participate;
        

        public static IForm<ConferenceEntry> BuildForm()
        {
            return new FormBuilder<ConferenceEntry>()
                    .Message("Register a new conference here")
                    .Build();
        }
    };
}