using System;

namespace Splits.Web.ModelBinding
{
  public class ConverterExpression
  {
    private readonly Predicate<Type> _matches;
    private readonly Action<ConverterFamily> _register;

    public ConverterExpression(Predicate<Type> matches, Action<ConverterFamily> register)
    {
      _matches = matches;
      _register = register;
    }

    public void Use(Func<IValueConverterRegistry, Type, ValueConverter> builder)
    {
      _register(new ConverterFamily(_matches, builder));
    }
  }
}