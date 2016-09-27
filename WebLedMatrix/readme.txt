Instructions:
-Enable all WCF options at Windows Features.
-Enable net.tcp protocol at iis service manager(service  right click -> manage website -> Advanced Settings)
-Set Bindings (Default Web Site -> Right Click -> Edit Bindings -> add net.tcp '808:*' )
If stil it doesnt works http://stackoverflow.com/questions/3188618/enabling-net-tcp-in-iis7
