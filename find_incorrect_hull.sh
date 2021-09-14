equal=true
seed=0

while $equal;
do
  equal=$(./chan.exe $seed)
  echo "$equal"
  echo "$seed"
  ((seed=seed+1))
  if [[ seed -eq 73 ]]; then
    ((seed=seed+1))
  fi
done
