using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassExtractor
{
    public record MemberDetails(
        string Visibility,
        string Type,
        string Name);

    public record MethodDetails(
        string Visibility,
        string ReturnType,
        string Name,
        List<ParameterDetails> Parameters);

    public record ParameterDetails(
        string Name,
        string Type,
        string Modifiers,
        string? DefaultValue);
    
}