rename-item -path sln\SharedInternalsVisibleTo.cs -newname SharedInternalsVisibleTo-backup.cs
$data = get-content -path sln\SharedInternalsVisibleToSigned.cs
new-item -path "sln\" -name "SharedInternalsVisibleTo.cs" -itemtype "file" -value "$data"