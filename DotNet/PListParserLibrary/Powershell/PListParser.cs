using System;
using System.Management.Automation;

namespace Powershell
{
    [Cmdlet(VerbsCommon.Get, "PlistFromText")]
    [OutputType(typeof(PSPrimitiveDictionary))]
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
                var pwOutput = new PSPrimitiveDictionary();
                foreach (var kv in output)
                {
                    pwOutput.Add(kv.Key, kv.Value);
                }
                WriteObject(pwOutput);
            }

        }
    }
}

