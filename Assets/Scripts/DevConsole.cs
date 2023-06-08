using TMPro;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using System.Collections;
using System.Reflection;


public class DevConsole : MonoBehaviour
{
    public static DevConsole Instance { get; set; }
    public GameObject ConsolePanel;
    public GameObject CommandSuggestionPanel;
    public TextMeshProUGUI SuggestionTextPrefab;
    public TextMeshProUGUI ConsoleText;
    public TMP_InputField ConsoleInput;
    public Color ErrorColor;
    public Color NormalColor;
    public Color WarningColor;
    public Color CommandColor;
    bool isAutoCompleting = false;
    private List<TextMeshProUGUI> commandSuggestions = new List<TextMeshProUGUI>();

    [ShowInInspector]
    public Dictionary<string, Action<string[]>> CommandDictionary = new Dictionary<string, Action<string[]>>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // GetComponent<GamePanel>().OnPanelOpened += OnPanelOpened;
        ConsoleInput.onSubmit.AddListener(OnConsoleInput);
        ConsoleInput.onValueChanged.AddListener(OnConsoleValueChanged);
        AddCommandToDictionary("bg.color", ChangeBackgroundColor);
        AddCommandToDictionary("/commands", ListCommandsCommand);
        AddCommandToDictionary("/clear", ClearConsoleCommand);
        AddCommandToDictionary("/help", HelpCommand);

        // Add methods marked with DevConsoleCommand to the command dictionary
        foreach (var method in GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
        {
            var attributes = method.GetCustomAttributes(typeof(DevConsoleCommand), false);
            if (attributes.Length > 0)
            {
                var attribute = (DevConsoleCommand)attributes[0];
                AddCommandToDictionary(attribute.commandName, (Action<string[]>)Delegate.CreateDelegate(typeof(Action<string[]>), this, method));
            }
        }
    }

    private void OnPanelOpened()
    {
        ConsoleInput.Select();
        ConsoleInput.ActivateInputField();
    }

    public void OnConsoleInput(string input)
    {
        ConsoleText.text += "\n" + input;
        ConsoleInput.text = "";

        if (!CheckCommand(input))
        {
            PrintToConsole("Invalid command. Type <color=yellow>'/commands'</color> to see available commands.");
            //select the input field
            ConsoleInput.Select();

            //make sure the input field is selected
            ConsoleInput.ActivateInputField();


        }
        ConsoleInput.Select();
        ConsoleInput.ActivateInputField();


    }

    public void ToggleConsole()
    {
        ConsolePanel.SetActive(!ConsolePanel.activeSelf);
    }

    //method to check if the command is valid
    public bool CheckCommand(string command)
    {
        //split the command into an array
        string[] splitCommand = command.Split(' ');
        //check if the command is valid
        if (splitCommand.Length > 0)
        {
            //check if the command is in the dictionary
            if (CommandDictionary.ContainsKey(splitCommand[0]))
            {
                //run the command
                CommandDictionary[splitCommand[0]].Invoke(splitCommand);
                return true;
            }
        }
        return false;
    }

    //method to add a command to the dictionary
    public void AddCommandToDictionary(string command, Action<string[]> method)
    {
        //check if the command is valid
        if (string.IsNullOrEmpty(command))
        {
            Debug.LogError("Command is null or empty");
            return;
        }
        //check if the method is valid
        if (method == null)
        {
            Debug.LogError("Method is null");
            return;
        }
        //check if the command is already in the dictionary
        if (CommandDictionary.ContainsKey(command))
        {
            Debug.LogError("Command is already in the dictionary");
            return;
        }
        //add the command to the dictionary
        CommandDictionary.Add(command, method);
    }

    //method to remove a command from the dictionary
    public void RemoveCommandFromDictionary(string command)
    {
        //check if the command is valid
        if (string.IsNullOrEmpty(command))
        {
            Debug.LogError("Command is null or empty");
            return;
        }
        //check if the command is in the dictionary
        if (CommandDictionary.ContainsKey(command) == false)
        {
            Debug.LogError("Command is not in the dictionary");
            return;
        }
        //remove the command from the dictionary
        CommandDictionary.Remove(command);
    }

    //method to clear the console
    public void ClearConsole()
    {
        ConsoleText.text = "";
    }

    //method to print to the console
    public void PrintToConsole(string message)
    {
        //if the message is empty, return
        if (String.IsNullOrEmpty(message))
        {
            return;
        }
        var originalMessage = message;
        //parse the message for any commands
        var commands = ParseForCommands(message);

        // Debug.Log($"Original message: {originalMessage}");
        // Debug.Log($"Parsed message: {string.Join(" ", commands.ToArray())}");

        ConsoleText.text += "\n" + message;
    }

    public List<string> ParseForCommands(string message)
    {
        //return a list of strings that contain commands from the command dictionary
        List<string> parsedMessage = new List<string>();

        //split the message into an array
        string[] splitMessage = message.Split(' ');

        //loop through all words in the message

        foreach (string word in splitMessage)
        {
            //check if the word is a command
            if (CommandDictionary.ContainsKey(word))
            {
                //add the command to the list
                parsedMessage.Add($"<color=blue>{word}</color>");
            }
            else
            {
                //add the word to the list
                parsedMessage.Add(word);
            }
        }

        return parsedMessage;
    }

    //method to print to the console
    public void PrintToConsole(string message, Color color)
    {
        var oldColor = ConsoleText.color;
        //set the color of the text
        ConsoleText.color = color;

        ConsoleText.text += "\n" + message;
        ConsoleText.color = oldColor;
    }
    //method to list all commands in the dictionary
    public void ListCommands()
    {
        //loop through all commands in the dictionary
        foreach (KeyValuePair<string, Action<string[]>> command in CommandDictionary)
        {
            //print the command to the console
            PrintToConsole(command.Key);
        }
    }

    void ChangeBackgroundColor(string[] args)
    {
        if (args.Length < 4)
        {
            DevConsole.Instance.PrintToConsole("Invalid arguments. Usage: change_background_color R G B");
            return;
        }

        float r, g, b, a;

        if (float.TryParse(args[1], out r) && float.TryParse(args[2], out g) && float.TryParse(args[3], out b) && float.TryParse(args[4], out a))
        {
            var consolepanelimage = ConsolePanel.transform.GetChild(0).GetComponent<Image>();
            consolepanelimage.color = new Color(r, g, b, a);
            DevConsole.Instance.PrintToConsole($"Background color changed to: ({r}, {g}, {b}, {a})");
        }
        else
        {
            DevConsole.Instance.PrintToConsole("Invalid arguments. Please enter valid numbers for R, G, B, A");
        }
    }

    private void OnConsoleValueChanged(string input)
    {
        UpdateCommandSuggestions(input);
    }

    private void UpdateCommandSuggestions(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            ClearCommandSuggestions();
            CommandSuggestionPanel.SetActive(false);
            return;
        }

        List<string> suggestions = new List<string>();

        foreach (var command in CommandDictionary.Keys)
        {
            if (command.StartsWith(input, StringComparison.OrdinalIgnoreCase))
            {
                suggestions.Add(command);
            }
        }

        if (suggestions.Count > 0)
        {
            CommandSuggestionPanel.SetActive(true);
            PopulateCommandSuggestions(suggestions);
        }
        else
        {
            CommandSuggestionPanel.SetActive(false);
        }
    }

    private void ClearCommandSuggestions()
    {
        foreach (var suggestion in commandSuggestions)
        {
            Destroy(suggestion.gameObject);
        }

        commandSuggestions.Clear();
    }

    private void PopulateCommandSuggestions(List<string> suggestions)
    {
        ClearCommandSuggestions();

        // Calculate the caret position within the TMP_InputField text area
        int cursorIndex = ConsoleInput.stringPosition;
        TMP_CharacterInfo charInfo = ConsoleInput.textComponent.textInfo.characterInfo[cursorIndex];
        Vector3 charMidBaselinePos = charInfo.bottomLeft + (charInfo.topRight - charInfo.bottomLeft) / 2;

        // Convert the caret position to world position
        RectTransform consoleInputRectTransform = ConsoleInput.textComponent.rectTransform;
        Vector3 worldCaretPos = consoleInputRectTransform.TransformPoint(charMidBaselinePos);

        // Position the CommandSuggestionPanel just below the caret position
        CommandSuggestionPanel.transform.position = worldCaretPos;// + new Vector3(0, -CommandSuggestionPanel.GetComponent<RectTransform>().rect.height/2, 0); //-CommandSuggestionPanel.GetComponent<RectTransform>().rect.height, 0);


        //loop
        foreach (var suggestion in suggestions)
        {
            TextMeshProUGUI newText = Instantiate(SuggestionTextPrefab, CommandSuggestionPanel.transform);
            newText.text = suggestion;
            newText.GetComponent<Button>().onClick.AddListener(() => SelectSuggestion(suggestion));
            commandSuggestions.Add(newText);
        }
    }

    public void SelectSuggestion(string suggestion)
    {
        ConsoleInput.text = suggestion;
        ConsoleInput.MoveTextEnd(false);
        ConsoleInput.Select();

        ConsoleInput.caretPosition = ConsoleInput.text.Length;
        CommandSuggestionPanel.SetActive(false);
    }


    public void AutoCompleteCommand(string command)
    {
        //wait to autocomplete if the command has more than 4 characters
        if (command.Length < 4 || isAutoCompleting)
        {
            return;
        }

        //loop through all commands in the dictionary
        foreach (KeyValuePair<string, Action<string[]>> commandInDictionary in CommandDictionary)
        {
            //check if the command starts with the input
            if (commandInDictionary.Key.StartsWith(command))
            {
                isAutoCompleting = true;

                //if tab is pressed, autocomplete the command
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    ConsoleInput.text = commandInDictionary.Key;

                    //set the caret position to the end of the input
                    StartCoroutine(SetCaretPositionToEnd());

                    //break out of the loop
                    isAutoCompleting = false;
                    break;
                }
            }
        }
    }

    private IEnumerator SetCaretPositionToEnd()
    {
        // Wait until next frame to set the caret position
        yield return null;
        ConsoleInput.caretPosition = ConsoleInput.text.Length;
    }

    private void ListCommandsStartingWithInput(string command)
    {
        //list commands that start with the input
        foreach (KeyValuePair<string, Action<string[]>> commandInDictionary in CommandDictionary)
        {
            //check if the command starts with the input
            if (commandInDictionary.Key.StartsWith(command))
            {
                //print the command to the console
                PrintToConsole(commandInDictionary.Key);
            }
        }
    }

    //command to clear the console
    void ClearConsoleCommand(string[] args)
    {
        ClearConsole();
    }

    //command to list all commands
    void ListCommandsCommand(string[] args)
    {
        ListCommands();
    }

    void HelpCommand(string[] obj)
    {
        PrintToConsole("List of commands:");
        ListCommands();
    }

}


[System.AttributeUsage(System.AttributeTargets.Method)]
public class DevConsoleCommand : System.Attribute
{
    public string commandName;

    public DevConsoleCommand(string commandName)
    {
        this.commandName = commandName;
    }
}
