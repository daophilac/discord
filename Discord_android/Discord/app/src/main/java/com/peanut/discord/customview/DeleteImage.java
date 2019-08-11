package com.peanut.discord.customview;
import android.content.Context;
import android.util.AttributeSet;

import androidx.appcompat.widget.AppCompatImageView;

import com.peanut.discord.R;
public class DeleteImage extends AppCompatImageView {
    public DeleteImage(Context context) {
        super(context);
    }
    public DeleteImage(Context context, AttributeSet attrs) {
        super(context, attrs, R.attr.deleteImageStyle);
    }
    public DeleteImage(Context context, AttributeSet attrs, int defStyleAttr) {
        super(context, attrs, defStyleAttr);
    }
}
