package com.daophilac.discord.customview;


import android.content.Context;
import android.support.v7.widget.AppCompatButton;
import android.util.AttributeSet;
import android.view.View;
import android.widget.Button;

import com.daophilac.discord.MainActivity;

public class ServerButton extends AppCompatButton{
    private int serverID;
    public ServerButton(Context context) {
        super(context);
    }

    public ServerButton(Context context, AttributeSet attrs) {
        super(context, attrs);
    }

    public ServerButton(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
    }
    public void setServerID(int serverID){
        this.serverID = serverID;
    }
    public int getServerID(){
        return this.serverID;
    }
}
