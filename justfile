bash := if os_family() == 'windows' {
    'C:\Program Files\Git\usr\bin\bash.exe'
} else {
    '/usr/bin/env bash'
}

new-xunit NAME:
    #!{{bash}}
    export DOTNET_ROOT="$HOME/.dotnet"
    dotnet new gnome-xunit -n "Gnome.{{ NAME }}.Tests" -o "./lib/{{ NAME }}/tests" --force
   
add-ref PROJ REF:
    #!{{bash}}
    export DOTNET_ROOT="$HOME/.dotnet"
    dotnet add "./lib/{{ PROJ }}/src" reference "./lib/{{ REF }}/src"

add-test-ref PROJ REF:
    #!{{bash}}
    export DOTNET_ROOT="$HOME/.dotnet"
    dotnet add "./lib/{{ PROJ }}/tests" reference "./lib/{{ REF }}/src"

new-std NAME:
    #!{{bash}}
    export DOTNET_ROOT="$HOME/.dotnet"
    dotnet new gnome-stdlib -n "Gnome.{{ NAME }}" -o "./lib/{{ NAME }}/src" \
        --license \
        --changelog \
        --no-framework \
        --use-icon-prop \
        --polyfill 
    dotnet new sln -n "{{ NAME }}" -o "./lib/{{ NAME }}"
    dotnet new gnome-xunit -n "Gnome.{{ NAME }}.Tests" -o "./lib/{{ NAME }}/test"
    dotnet add "./lib/{{ NAME }}/test" reference "./lib/{{ NAME }}/src"
    dotnet sln "./lib/{{ NAME }}"  add  "./lib/{{ NAME }}/src" 
    dotnet sln "./lib/{{ NAME }}"  add  "./lib/{{ NAME }}/test" 
    dotnet sln . add "./lib/{{ NAME }}/src"
    dotnet sln . add "./lib/{{ NAME }}/test"
    printf "# Gnome.{{ NAME }}\n" >> "./lib/{{ NAME }}/README.md"
    

new-unsafe NAME:
    #!{{bash}}
    export DOTNET_ROOT="$HOME/.dotnet"
    dotnet new gnome-stdlib -n "Gnome.{{ NAME }}" -o "./src/{{ NAME }}" \
        --license \
        --changelog \
        --no-framework \
        --polyfill \
        --unsafe \
        --use-icon-prop \
        --no-restore
    dotnet sln add "./src/{{ NAME }}"

add-migration MIGRATION: 
    #!{{bash}}
    export DOTNET_ROOT="$HOME/.dotnet"
    dotnet-ef migrations add {{ MIGRATION }} --project "./src/Data.Migrations.Sqlite/Gs.Data.Migrations.Sqlite.csproj"  --output-dir "Migrations/Sqlite" --context "SqliteGsDbContext" --verbose 
    dotnet-ef migrations add {{ MIGRATION }} --project "./src/Data.Migrations.Pgsql/Gs.Data.Migrations.Pgsql.csproj" --output-dir "Migrations/Pgsql" --context "PgsqlGsDbContext" --verbose 
    dotnet-ef migrations add {{ MIGRATION }} --project "./src/Data.Migrations.Mssql/Gs.Data.Migrations.Mssql.csproj" --output-dir "Migrations/Mssql" --context "MssqlGsDbContext" --verbose 
    