using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyCSharpPreprocessor
{
    internal class CommandLineOptions
    {
        internal bool DeleteUnusedClasses { get; private set; }
        internal bool RenameClasses { get; private set; }
        internal bool ChangeAllPrivateMembersToInternal { get; private set; }
        internal bool OverwriteInputFile { get; private set; }

        internal static CommandLineOptions Parse(string[] args)
        {
            CommandLineOptions options = new CommandLineOptions();
            foreach (string arg in args)
            {
                switch (arg)
                {
                    case "-d":
                    case "--delete-unused-classes":
                        options.DeleteUnusedClasses = true;
                        break;
                    case "-r":
                    case "--rename-classes":
                        options.RenameClasses = true;
                        break;
                    case "-c":
                    case "--change-all-private-members-to-internal":
                        options.ChangeAllPrivateMembersToInternal = true;
                        break;
                    case "-o":
                    case "--overwrite-file":
                        options.OverwriteInputFile = true;
                        break;
                }
            }
            return options;
        }
    }
}
