package com.daophilac.discord.customview;


import android.content.Context;
import android.support.v7.widget.AppCompatTextView;
import android.util.AttributeSet;
import android.widget.TextView;

public class ChannelTextView extends AppCompatTextView {
    private int channelID;
    public ChannelTextView(Context context) {
        super(context);
    }

    public ChannelTextView(Context context, AttributeSet attrs) {
        super(context, attrs);
    }

    public ChannelTextView(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
    }

    public int getChannelID() {
        return channelID;
    }

    public void setChannelID(int channelID) {
        this.channelID = channelID;
    }
}
