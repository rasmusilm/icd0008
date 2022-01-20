dotnet aspnet-codegenerator razorpage -m Person -dc ApplicationDbContext -udl -outDir Pages/Persons --referenceScriptLibraries -f

dotnet aspnet-codegenerator razorpage -m Course -dc ApplicationDbContext -udl -outDir Pages/Courses --referenceScriptLibraries -f
dotnet aspnet-codegenerator razorpage -m CourseDeclaration -dc ApplicationDbContext -udl -outDir Pages/CourseDeclarations --referenceScriptLibraries -f
dotnet aspnet-codegenerator razorpage -m Grade -dc ApplicationDbContext -udl -outDir Pages/Grades --referenceScriptLibraries -f
dotnet aspnet-codegenerator razorpage -m Homework -dc ApplicationDbContext -udl -outDir Pages/Homeworks --referenceScriptLibraries -f
dotnet aspnet-codegenerator razorpage -m Recipe -dc DatabaseContext -udl -outDir Pages/Recipes --referenceScriptLibraries -f