Instructions:
-Enable all WCF options at Windows Features.
-Enable net.tcp protocol at iis service manager(service  right click -> manage website -> Advanced Settings)
-Set Bindings (Default Web Site -> Right Click -> Edit Bindings -> add net.tcp '808:*' )