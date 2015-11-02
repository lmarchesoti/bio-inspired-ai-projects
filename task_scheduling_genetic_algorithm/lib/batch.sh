#!bin/bash
NEXECS=20
CUREXEC=0
temp=8008
menor=8008

while [ $CUREXEC -lt $NEXECS ]
do
	temp=$(./prog)
	if [ $temp -lt $menor ]
	then
		menor=$temp
	fi
	CUREXEC=$(($CUREXEC+1))
done

echo $menor
