using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using NUnitLite;

class Program
{
	static void Main(string[] args)
	{
        new AutoRun().Execute(args);

        //var enterprise = new Enterprise
        //{
        //    Name = "Vector",
        //    DirectorName = "Jones",
        //    RegistrationNumber = "123"
        //};

        //List<LabelAttribute> gfd = new List<LabelAttribute>();
        // foreach (var e in enterprise.GetType().GetProperties())
        //{
        //    var attribute = e.GetCustomAttributes(true);        
        //}
    }
}

class LabelAttribute : Attribute
{
    public string LabelText { get; set; }
    public LabelAttribute(string labelText)
    {
        LabelText = labelText;
    }
}


class Enterprise
{
    [Label("�������� �����������")]
    public string Name { get; set; }
    [Label("��� ���������")]
    public string DirectorName { get; set; }
    [Label("��������������� �����")]
    public string RegistrationNumber { get; set; }
}

