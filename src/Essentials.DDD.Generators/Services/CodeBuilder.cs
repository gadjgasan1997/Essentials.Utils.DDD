namespace Essentials.DDD.Generators.Services;

internal class CodeBuilder
{
    private const string SPACE = " ";
    private const string INDENT = "    ";
    private readonly StringBuilder _builder;
    
    public CodeBuilder(string? code = null)
    {
        _builder = code == null ? new StringBuilder() : new StringBuilder(code);
    }
    
    public CodeBuilder AppendUsing(string directive)
    {
        Append("using").AppendSpace().Append(directive);
        return AppendSemicolon().AppendLine();
    }
    
    public CodeBuilder AppendNamespace(string @namespace)
    {
        Append("namespace").AppendSpace().Append(@namespace);
        return AppendSemicolon().AppendLine();
    }
    
    public CodeBuilder Append(string code)
    {
        _builder.Append(code);
        return this;
    }
    
    public CodeBuilder AppendIf(string code, bool condition) => AppendIf(() => code, condition);
    
    public CodeBuilder AppendIf(Func<string> codeFactory, bool condition)
    {
        if (!condition)
            return this;
        
        Append(codeFactory());
        return this;
    }
    
    public CodeBuilder AppendWithSpace(string code)
    {
        return Append(code).AppendSpace();
    }
    
    public CodeBuilder AppendSpace()
    {
        Append(SPACE);
        return this;
    }
    
    public CodeBuilder AppendIndent(int depth = 1)
    {
        for (var i = 0; i < depth; i++)
            Append(INDENT);
        return this;
    }
    
    public CodeBuilder AppendSemicolon()
    {
        Append(";");
        return this;
    }
    public CodeBuilder AppendComma()
    {
        AppendWithSpace(",");
        return this;
    }
    
    public CodeBuilder AppendOperator(string operatorName)
    {
        AppendWithSpace("operator").Append(operatorName);
        return this;
    }
    
    public CodeBuilder AppendLine()
    {
        _builder.AppendLine();
        return this;
    }
    
    public CodeBuilder AppendLine(string code)
    {
        Append(code).AppendLine();
        return this;
    }
    
    public CodeBuilder AppendCodeBlock(Action<CodeBuilder> action, bool needIndent = false)
    {
        AppendLine();
        
        if (needIndent)
            AppendIndent();
        
        AppendLine("{");
        
        var builder = new CodeBuilder();
        action(builder);
        
        var result = builder.Build();
        Append(result).AppendLine();
        
        if (needIndent)
            AppendIndent();
        
        return Append("}");
    }
    
    public string Build() => _builder.ToString();
}