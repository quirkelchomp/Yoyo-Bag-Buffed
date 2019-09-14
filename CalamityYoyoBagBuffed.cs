using Terraria.ModLoader;

namespace CalamityYoyoBagBuffed
{
    public class CalamityYoyoBagBuffed : Mod
    {
        // VARIABLES are named values that can be changed. Variable definitions consist of, at minimum, a TYPE (int, in the example below) and a name.
        //public int example_integer;  // This is called the VARIABLE DEFINITION. Here, the variable is called "Example Integer" and is of the type 'int'
        // Some types are nullable, meaning their values can be null (think n/a, essentially). Here, the example_integer is null right after its definition. If you tried to decrease or increase its value at this step, the program would give you a fatal error known as the NullReference Exception.
        // Fortunately, we can do something about this by initializing the variable.
        //example_integer = 0;  // This is called the VARIABLE INITIALIZATION
        // For the sake of convenience, this can be done in the same step as the variable definition, as on the last line of the code example as seen below.
        //public int example2 = 2;  // Here, we define a variable and immediately initialize it with the value "2".
        // Variable types can either be PRIMITIVE types, or they can be CLASSES. The int example is a primitive type. They can be distinguished from classes by the fact that classes are normally capitalized and highlighted in cyan, whereas primitive types are highlighted in blue.
        // For general modding, you will only need these primitive types: string, int, long, double, float, and bool.
        public CalamityYoyoBagBuffed()
        {
        }
        // Say we wanted to increase the knockback of an item based on its damage. The intuitive thing to do would be to write the following:
        //item.damage += item.knockBack;
        // However, this will not compile, as item.damage is an int value, and item.knockback is a float value. To fix this, we will do something known as TYPE CASTING (also sometimes referred to as PARSING, which is a slightly broader term)
        //item.damage += (int)(item.knockBack);
        // Doing this will convert the float value to an int value, allowing us to add it to the damage. This will also round down the value to a whole number.

    }
}
//namespace TutorialProject  // This is the NAMESPACE, or name of the project. In Modding, this will usually be the name of the Mod. They are similar to file addresses in Windows, but unlike in windows, the folders are separated by dots rather than slashes, for example: System.Exception
//{
//    public class ClassTutorial  // This is the CLASS DEFINITION. It is often preceded by an access keyword, such as public or private or internal, but for the purposes of general modding, you will use the 'public' keyword.
//    {
//        public ClassTutorial()  // This is the CONSTRUCTOR. It is a METHOD.
//        {
//            // Empty constructor-body.
//        }
//    }
//}

// As mentioned above, classes are a form of variable "types". Sometimes, a primitive type simply is not sufficient for our purposes when creating variables. Let’s use the Item class as an example:
//public Item i;
// As stated earlier, this is a variable definition. Unlike earlier, this variable uses a class as a type. Whenever an item is dropped, crafted, gathered or generated, a variable of the type Item is created.
// This is called INSTANTIATING, or making an INSTANCE of the class. The player inventory is a long array of Item-variables. Variables that are instances of a class are called OBJECTS.

// Objects are at the core of C#. The biggest difference between objects and primitive variables is that objects can contain their own variables, including other objects. Following along with the public Item i; example, we could do this:
//int dmg = i.damage;
// Here, we reference the variable 'damage', which is local to objects of the type Item. This will give us another NullReference Exception, though, because like variables, an empty definition like public Item i; will point to null.
// So how do we initialize an object, you might wonder? We do this by using a CONSTRUCTOR.
//      public ClassTutorial example_object = new ClassTutorial();
//      {
//      }
// So what is a constructor exactly? To answer that, we will have to first explore the topic of methods: In a C# program, all the code happens within methods.
// In fact, there’s a single parent Method from which all the code starts, known as the Main() method. This is unimportant for Terraria modding, but important if you want to do a standalone program.
// Methods play two main roles in code: They are defined and they are called. A method is essentially a procedure that is defined and can be repeated at will without typing the whole thing again.
// The method definition as a sort of recipe describing a procedure that happens every time the method is called.

// "string" (seen below) is the RETURN type
//public static string ExampleMethod(int a, int b, Random random) // Looks somewhat similar to a variable definition
//{  // everything within the { curly brackets } constitutes the METHOD BODY
//    string s = "";
//    s += a.ToString();
//    s += random.Next(b).ToString();
//    return s += " ~Anonymous String"
//}
// A method has a list of keywords, (in the example, public and static), a return type, a method name (ExampleMethod), a list of arguments, and a method body.
// The content within the parentheses (next to the method name) is method input, known as ARGUMENTS. These arguments are labeled like variables and can be referenced in the METHOD BODY. They cannot be changed, however, unless preceded by the keyword ref in both the method definition and call.
// You will also notice a new keyword, static. Variables and methods can be either static or nonstatic. If they are nonstatic, you need to reference or call it from an object, like so:
//i.damage;
// You can think of these as local variables, because each object has their own copy of that variable and can have different values assigned to it. In some cases, this is not desired. For those situations, we use the static keyword.
// “Local variable” means something else in C# though. A local variable is one which is defined inside a method, which means you cannot reference or change it outside of the same method. By contrast, variables declared directly in the class structure are called FIELDS.

// Here the method is called, meaning that all the code in the method body is run using the arguments(input) enclosed in the brackets.This saves you the trouble of rewriting the entire process.
//string output = ExampleMethod(4, 7, new Random());

// Now we will examine the concept known as RETURN types. In the ExampleMethod definition, the return type is string. Return types allow us to reference the method like a variable, and assign variables to the return. Now let us return to the constructor example:
//    public ClassTutorial example_object = new ClassTutorial();
// Despite being methods, constructors are not actually named. “ClassTutorial” is in fact the return type rather than a name. This is why we can use a constructor to initialize an object, because the constructor actually creates a new object and returns it when called.
// Since the constructor has no name, it uses a distinct keyword: new
// This example constructor takes no arguments and does not actually do anything, but if we’d like, we could add some code to the method body. Constructors are commonly used to initialize variables, so we will add a variable:
//namespace TutorialProject  
//{
//    public class ClassTutorial  // This is the CLASS DEFINITION.
//    {
//        public int index;
//        public ClassTutorial(int index)  // This is the CONSTRUCTOR.
//        {
//            this.index = index;
//        }
//    }
//}
// You will notice the object referenced as 'this', referring to the object being created. You do not actually need to put 'this', as it is implicitly understood, except when there’s an argument of the same name as the local variable, like in the example.
// Now that we have a local variable that gets initialized, we can reference it using any object of the type ClassTutorial, like so:
//      int index = new ClassTutorial(14).index;
// However, having not stored the object in a variable, all we’ve done is invented a more complicated way to initialize an int, so next we’ll invent a way to retrieve the ClassTutorial object from its index, using something known as arrays.

// Defining an array:
//public int example_array = new int[8];
// We have now created an array with a length of 8, meaning its indices go from zero to seven, for a total of eight int values

// Referencing the values of an array
//public int value = example_array[2]
// To reference a value in an array, you will need to put its index in square brackets after the array's name.