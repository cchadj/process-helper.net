import sys

print("Hello from python.")
print "Arguments provided are: " , str(sys.argv[1:])
sys.stderr.write("This is written in stderr from python")