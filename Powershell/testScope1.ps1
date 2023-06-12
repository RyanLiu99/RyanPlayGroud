Param(

	[string]$version 
)

$g = '1';
"g in 1 is $g"

$x1 = 'x1'
"x1 in 1 in $x1"
./testScope11.ps1

"after call 11, g in 1 is $g"

"x1 whihc changed in child, in 1 is $x1"

"Para versin in 1 is  $version"