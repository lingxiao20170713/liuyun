//
//  WxPlatformInfo.m
//  Unity-iPhone
//
//  Created by mac on 16/12/22.
//
//
#import <Foundation/Foundation.h>

#if defined(_cplusplus)
extern "C"
{
#endif
    
    const char* GetAppName_Unity()
    {
        NSDictionary* dict = [[NSBundle mainBundle] infoDictionary];
        NSString* appName = [dict objectForKey:@"CFBundleDisplayName"];
        const char* str = [appName UTF8String];
        
        char* ret = malloc(strlen(str) + 1);
        strcpy(ret, str);
        
        return ret;
    }
    
    const char* GetPackageName_Unity()
    {
        NSDictionary* dict = [[NSBundle mainBundle] infoDictionary];
        NSString* appName = [dict objectForKey:@"CFBundleIdentifier"];
        const char* str = [appName UTF8String];
        
        char* ret = malloc(strlen(str) + 1);
        strcpy(ret, str);
        
        return ret;
    }
    
    const char* GetVersion_Unity()
    {
        NSDictionary* dict = [[NSBundle mainBundle] infoDictionary];
        NSString* appName = [dict objectForKey:@"CFBundleVersion"];
        const char* str = [appName UTF8String];
        
        char* ret = malloc(strlen(str) + 1);
        strcpy(ret, str);
        
        return ret;    }
    
    const char* GetVersionName_Unity()
    {
        NSDictionary* dict = [[NSBundle mainBundle] infoDictionary];
        NSString* appName = [dict objectForKey:@"CFBundleShortVersionString"];
        const char* str = [appName UTF8String];
        
        char* ret = malloc(strlen(str) + 1);
        strcpy(ret, str);
        
        return ret;
    }
    
    bool CopyTextToClipboard_Unity(const char* szText)
    {
        UIPasteboard* pasteboard = [UIPasteboard generalPasteboard];
        if (pasteboard != nil)
        {
            [pasteboard setString:[NSString stringWithCString:szText encoding:NSUTF8StringEncoding]];
            return true;
        }
        return false;
    }
    
    const char* GetTextFromClipboard_Unity()
    {
        UIPasteboard* pasteboard = [UIPasteboard generalPasteboard];
        if (pasteboard != nil)
        {
            NSString* str =pasteboard.string;
            if (str != nil && str.length > 0)
            {
                const char* cstr = [str UTF8String];
                
                char* ret = malloc(strlen(cstr) + 1);
                strcpy(ret, cstr);
                
                return ret;

            }
        }
        return false;
    }
    
    const char* GetFileContent_Unity(const char* szPath, const char* type)
    {
        NSString* nsPath = [[NSBundle mainBundle] pathForResource:[NSString stringWithCString:szPath encoding:NSUTF8StringEncoding] ofType:
                            [NSString stringWithCString:type encoding:kCFStringEncodingUTF8]];
        
        NSError* err = nil;
        NSString* content = [NSString stringWithContentsOfFile:nsPath encoding:NSUTF8StringEncoding error:&err];
        if (err != nil)
        {
            return NULL;
        }
        const char* cstr = [content UTF8String];
        
        char* ret = malloc(strlen(cstr) + 1);
        strcpy(ret, cstr);
        
        return ret;
    }
    
    void JumpRecomment_Unity()
    {
        [[UIApplication sharedApplication] openURL:[NSURL URLWithString:@"https://itunes.apple.com/hk/app/id1186178498"]];
    }


#if defined(_cplusplus)
}
#endif
