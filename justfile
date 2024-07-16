bash := if os_family() == 'windows' {
    'C:\Program Files\Git\usr\bin\bash.exe'
} else {
    '/usr/bin/env bash'
}

new-std NAME:
    #!{{bash}}
    export DOTNET_ROOT="$HOME/.dotnet"
    dotnet new gnome-stdlib -n "Gnome.{{ NAME }}" -o "./src/{{ NAME }}" \
        --license \
        --changelog \
        --no-framework \
        --use-icon-prop \
        --polyfill 
    dotnet sln . add "./src/{{ NAME }}"

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
    