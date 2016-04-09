using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomataConverter
{
    public class Application
    {
        List<ICommand> commands = new List<ICommand>();
        Dictionary<string, ICommand> commandMap = new Dictionary<string, ICommand>();
        Dictionary<string, string> synonymsMap = new Dictionary<string, string>();
        public Dictionary<char, object> variables { get; set; }
        public Application() 
        {
            variables = new Dictionary<char, object>();
        }
        public ICommand FindCommand(string name)
        {
            if (synonymsMap.ContainsKey(name))
            {
                return commandMap[synonymsMap[name]];
            }
            else
            {
                if (commandMap.ContainsKey(name))
                {
                    return commandMap[name];
                }
                else
                {
                    return null;
                }
            }
        }
        public void AddCommand(ICommand cmd)
        {
            commands.Add(cmd);
            commandMap[cmd.Name] = cmd;
            foreach(string word in cmd.Synonyms)
                synonymsMap[word] = cmd.Name;
        }
        public string[] VariableSubstitution(string[] parameters) 
        {
            IEnumerable<char>  allVariablesNames = variables.Keys;
            
            for (int i = 0; i < parameters.Length; i++) 
            {
                if(parameters[i].Length>1)
                {
                    continue;
                }
                if (allVariablesNames.Contains(parameters[i][0])) 
                {
                    char variableName =parameters[i][0];
                    object variableValue=variables[variableName];
                    if (variableValue is int)
                    {
                        parameters[i] = ((int)variableValue).ToString();
                    }
                    else if (variableValue is string) 
                    {
                        parameters[i] = variableValue as string;
                    }
                    else if (variableValue is int[]) 
                    {

                        string[] intsAsStrings = new string[(variableValue as int[]).Length];
                        for (int j = 0; j < intsAsStrings.Length; j++) 
                        {
                            intsAsStrings[j]=(variableValue as int[])[j].ToString();
                        }

                        List<string> parametersList = parameters.ToList<string>();
                        parametersList.RemoveAt(i);
                        parametersList.InsertRange(i, intsAsStrings);
                        i += intsAsStrings.Length - 1;
                    }
                }
            }
            return parameters;
        }
        public void Run()
        {
            string [] cmdline;
            string [] parameters;
            while (true)
            {
                cmdline = Console.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (cmdline.Length > 1)
                {
                    parameters = new string[cmdline.Length - 1];
                    Array.Copy(cmdline, 1, parameters, 0, parameters.Length);
                }
                else
                {
                    parameters = new string[] { };
                }
                if (cmdline.Length != 0)
                {
                    try
                    {
                        FindCommand(cmdline[0]).Execute(parameters);

                    }
                    catch (NullReferenceException)
                    {
                        Console.WriteLine("Эта команда не найдена. Наберите help для просмотра справки о командах");
                    }
                }
            }
        }
        public IList<ICommand> Commands { get { return commands; } }

    }
}
