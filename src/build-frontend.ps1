Set-EnvironmentVariable -Name NODE_OPTIONS -Value '--max_old_space_size=8192' -Scope Process
$path = $PWD
Set-Location frontend
npm install
npm run build
Set-Location $path