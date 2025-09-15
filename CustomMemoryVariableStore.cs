using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Yarn;

namespace Balance;

//
// Summary:
//     A simple concrete implementation of Yarn.IVariableStorage that keeps all variables
//     in memory.
public class CustomMemoryVariableStore : IVariableStorage, IVariableAccess
{
    public Dictionary<string, object> DialogueVariables = new Dictionary<string, object>();

    public Yarn.Program? Program { get; set; }

    public ISmartVariableEvaluator? SmartVariableEvaluator { get; set; }

    private static bool TryGetAsType<T>(Dictionary<string, object> dictionary, string key, [NotNullWhen(true)] out T? result)
    {
        if (dictionary.TryGetValue(key, out object value) && typeof(T).IsAssignableFrom(value.GetType()))
        {
            result = (T)value;
            return true;
        }

        result = default(T);
        return false;
    }

    public virtual bool TryGetValue<T>(string variableName, [NotNullWhen(true)] out T? result)
    {
        if (Program == null)
        {
            throw new InvalidOperationException("Can't get variable " + variableName + ": Program is null");
        }

        switch (GetVariableKind(variableName))
        {
            case VariableKind.Stored:
                if (TryGetAsType<T>(DialogueVariables, variableName, out result))
                {
                    return true;
                }

                return Program.TryGetInitialValue<T>(variableName, out result);
            case VariableKind.Smart:
                if (SmartVariableEvaluator == null)
                {
                    throw new InvalidOperationException("Can't get variable " + variableName + ": SmartVariableEvaluator is null");
                }

                return SmartVariableEvaluator.TryGetSmartVariable<T>(variableName, out result);
            default:
                result = default(T);
                return false;
        }
    }

    public void Clear()
    {
        DialogueVariables.Clear();
    }

    public virtual void SetValue(string variableName, string stringValue)
    {
        DialogueVariables[variableName] = stringValue;
    }

    public virtual void SetValue(string variableName, float floatValue)
    {
        DialogueVariables[variableName] = floatValue;
    }

    public virtual void SetValue(string variableName, bool boolValue)
    {
        DialogueVariables[variableName] = boolValue;
    }

    public VariableKind GetVariableKind(string name)
    {
        if (DialogueVariables.ContainsKey(name))
        {
            return VariableKind.Stored;
        }

        if (Program == null)
        {
            return VariableKind.Unknown;
        }

        return Program.GetVariableKind(name);
    }
}