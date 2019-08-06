package com.peanut.discord.customview;

import android.content.Context;

import androidx.annotation.NonNull;
import androidx.annotation.Nullable;
import androidx.recyclerview.widget.RecyclerView;
import android.util.AttributeSet;

import com.peanut.discord.R;

public class ChannelRecyclerView extends RecyclerView {
    public ChannelRecyclerView(@NonNull Context context) {
        super(context);
    }

    public ChannelRecyclerView(@NonNull Context context, @Nullable AttributeSet attrs) {
        super(context, attrs, R.attr.channelRecyclerViewStyle);
//        TypedArray typedArray = context.obtainStyledAttributes(attrs, R.styleable.ChannelRecyclerView);
//        int styleId = typedArray.getResourceId(R.styleable.ChannelRecyclerView_channelRecyclerViewStyle, -1);
//        if(styleId != 1){
//            typedArray = context.obtainStyledAttributes(styleId, new int[]{R.attr.backgroundColor, R.attr.textColor});
//            setBackgroundColor(typedArray.getColor(R.styleable.ChannelRecyclerView_backgroundColor, -1));
//        }
//        typedArray.recycle();
    }

    public ChannelRecyclerView(@NonNull Context context, @Nullable AttributeSet attrs, int defStyle) {
        super(context, attrs, defStyle);
    }

}