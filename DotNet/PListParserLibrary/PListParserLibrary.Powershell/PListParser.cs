using System.Collections.Generic;
using System.Management.Automation;

namespace PListParserLibrary.Powershell
{
    [Cmdlet(VerbsCommon.Get, "PlistFromText")]
    [OutputType(typeof(Dictionary<string, object>))]
    public class PListParser : PSCmdlet
    {
        [Parameter(Position = 0, ValueFromPipelineByPropertyName = true)]
        public string Input { get; set; }

        protected override void BeginProcessing()
        {
            base.BeginProcessing();
        }
        protected override void ProcessRecord()
        {
            if (Input is not null)
            {
                var plistParserLibrary = new PListParserLibrary.Parser();
                var output = plistParserLibrary.Parse(Input);
                WriteObject(output);
            }
        }
    }
}

