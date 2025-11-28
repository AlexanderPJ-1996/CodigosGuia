# .NET:
    -> dotnet                                                      <--> .NET
	-> dotnet new                                                  <--> Plantillas más comunes de proyectos .NET
	-> dotnet new install Avalonia.Templates                       <--> Descargar e instalar Avalonia UI para .NET
	-> dotnet new uninstall Avalonia.Templates                     <--> Desinstalar Avalonia UI para .NET
	-> dotnet new list                                             <--> Plantillas de proyectos .NET
	-> dotnet new list Avalonia                                    <--> Plantillas de proyectos Avalonia UI
	-> dotnet new sln                                              <--> Crear archivo sln, solución de proyecto
	-> dotnet new [Proyecto_Tipo] -n [Proyecto_Nombre]             <--> Crear archivo/proyecto asignando nombre
	-> dotnet new [Proyecto_Tipo] -n [Proyecto_Nombre] -f net#.0   <--> Crear archivo/proyecto asignando versión de .NET
	-> dotnet new globaljson --sdk-version #.#.###                 <--> Hacer que toda la solución use un SDK .NET
	-> dotnet list package --include-transitive         
	-> dotnet build --configuration Debug                          <--> Compilar en Debug
	-> dotnet build --configuration Release                        <--> Compilar en Release

# Visual Studio Community 2019 (PowerShell):
	-> MSBuild [Proyecto].sln /p:Configuration=Debug               <--> Compilar en Debug
	-> MSBuild [Proyecto].sln /p:Configuration=Release             <--> Compilar en Release

# 